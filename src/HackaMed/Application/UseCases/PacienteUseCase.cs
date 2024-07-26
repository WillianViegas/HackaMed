using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Application.UseCases.Interfaces;
using Domain.Entities;
using Domain.Helpers;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class PacienteUseCase : IPacienteUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IProntuarioRepository _prontuarioRepository;
        private readonly ILogger<PacienteUseCase> _log;
        private static IAmazonS3 _s3Client;
        private const string bucketName = "prontuario-bkt";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;

        public PacienteUseCase(IUsuarioRepository usuarioRepository, IProntuarioRepository prontuarioRepository, ILogger<PacienteUseCase> log, IAmazonS3 s3client)
        {
            _usuarioRepository = usuarioRepository;
            _prontuarioRepository = prontuarioRepository;
            _log = log;
            _s3Client = s3client;
        }
        public async Task<IList<Medico>> GetAllMedicos(MedicoFilter medicoFilter)
        {
            try
            {
                var usuarios = await _usuarioRepository.GetAllMedicos(medicoFilter);
                var medicos = new List<Medico>();

                foreach (var usuario in usuarios)
                {
                    var medico = new Medico()
                    {
                        Id = usuario.Id,
                        Nome = usuario.Nome,
                        Email = usuario.Email,
                        CRM = usuario.CRM,
                        Avaliacoes = "0",
                        DistanciaKM = "10km",
                        Especialidade = usuario.Especialidade
                    };

                    medicos.Add(medico);
                }


                return medicos;
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Prontuario> CadastrarProntuario(Prontuario prontuario)
        {
            try
            {
                var prontuarioExists = await _prontuarioRepository.GetProntuarioByPacienteId(prontuario.PacienteId);

                if (prontuarioExists != null && !string.IsNullOrWhiteSpace(prontuarioExists.Id))
                    throw new ValidationException("Prontuário já cadastrado para o paciente.");

                prontuario.DataCadastro = DateTime.Now;
                prontuario.DataAlteracao = DateTime.Now;

                return await _prontuarioRepository.CreateProntuario(prontuario);
            }
            catch (ValidationException ex)
            {
                _log.LogError(ex.Message);
                throw new ValidationException(ex.Message);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Prontuario> GetProntuarioByPacienteId(string pacienteId)
        {
            try
            {
                var prontuario = await _prontuarioRepository.GetProntuarioByPacienteId(pacienteId);
                return prontuario;
            }
            catch (ValidationException ex)
            {
                _log.LogError(ex.Message);
                throw new ValidationException(ex.Message);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Prontuario> AdicionarDocumento(Documento documento, IFormFile file)
        {
            try
            {
                var prontuario = await _prontuarioRepository.GetProntuarioByPacienteId(documento.PacienteId);

                if (prontuario == null || string.IsNullOrEmpty(prontuario.Id))
                    throw new ValidationException("Prontuario não encontrado");

                if (prontuario.Documentos.Any(x => x.Titulo == documento.Titulo))
                    throw new ValidationException("Já existe um documento com o mesmo titulo!");

                documento.DataCadastro = DateTime.Now;
                documento.DataAlteracao = DateTime.Now;


                var keyName = $"{documento.PacienteId}/{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var fileUrl = $"https://{bucketName}.s3.{bucketRegion.SystemName}.amazonaws.com/{keyName}";


                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = stream,
                        Key = keyName,
                        BucketName = bucketName,
                        CannedACL = S3CannedACL.Private, // Defina as permissões do objeto
                        ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256 // Criptografia no servidor
                    };

                    var fileTransferUtility = new TransferUtility(_s3Client);
                    await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
                }

                documento.Url = fileUrl;
                prontuario.Documentos.Add(documento);
                await _prontuarioRepository.UpdateProntuario(prontuario.Id, prontuario);

                return prontuario;
            }
            catch (ValidationException ex)
            {
                _log.LogError(ex.Message);
                throw new ValidationException(ex.Message);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Prontuario> RemoverDocumento(Documento documento)
        {
            try
            {
                var prontuario = await _prontuarioRepository.GetProntuarioByPacienteId(documento.PacienteId);

                if (prontuario == null || string.IsNullOrEmpty(prontuario.Id))
                    throw new ValidationException("Prontuario não encontrado");

                var doc = prontuario.Documentos.Where(x => x.Titulo == documento.Titulo).FirstOrDefault();

                if (doc == null)
                    throw new ValidationException("Documento não encontrado");


                prontuario.Documentos.Remove(doc);
                await _prontuarioRepository.UpdateProntuario(prontuario.Id, prontuario);

                return prontuario;
            }
            catch (ValidationException ex)
            {
                _log.LogError(ex.Message);
                throw new ValidationException(ex.Message);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
