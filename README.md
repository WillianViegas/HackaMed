# HackaMed

Repositório relacionado ao Hackathon FIAP. Proposta de sistema para consultas médicas online;
Repositório bloqueado para push na main, é necessário abrir um PullRequest;

Breve descrição da ideia do projeto:
Sistema para a realização de consultas online com o objetivo de permitir agendamento facilitado entre pacientes e médicos com foco em telemedicina.
Visando disponibilizar um sistema de fácil utilização, os médicos poderão cadastrar horários disponíveis em suas agendas, essas que ficaram disponíveis para os pacientes solicitarem consultas.
Cada paciente tem a possibilidade de cadastrar seu prontuário digital, mantendo documentos como carteira de vacinação, RG ou outras informações que possam auxiliar no processo de realização das consultas.
Ao receber uma solicitação de consulta o médico poderá aceitar ou rejeitar, as consultas aceitas irão gerar um link para ser realizada a consulta na data agendada.


# Proposta do Hackathon:
Com foco no desenvolvimento das habilidades de arquitetura de software, no hackathon foi solicitado o desenho do projeto na Cloud e estrutural com foco no futuro, montar uma arquitetura que possa ser expandida futuramente.
Além do desenho de arquitetura foi solicitado um MVP do projeto para exemplificar o funcionamento do mesmo, foi construído um monolito visando o rápido desenvolvimento de uma estrutura utilizável baseada nos requisitos funcionais e não funcionais solicitados:

## Resumo dos requisitos funcionais:
1. Autenticação do Usuário (Médico)
2. Cadastro/Edição de Horários Disponíveis (Médico)
3. Aceite ou Recusa de Consultas Médicas (Médico)
4. Autenticação do Usuário (Paciente)
5. Busca por Médicos (Paciente)
6. Agendamento de Consultas (Paciente)
7. Teleconsulta
8. Prontuário Eletrônico
 - Acesso e Upload:
 - Gestão de Compartilhamento:

## Resumo dos requisitos não funcionais:
1. Alta Disponibilidade
2. Escalabilidade
3. Segurança

## Estutura
 - Banco de dados = MongoDB
 - Serviços AWS utilizados dentro do projeto = S3 ( É utilizado para salvar os documentos do prontuário do paciente )
 - Serviços AWS utilizado fora do projeto = ECR, EKS, IAM, CloudWatch
 - Docker Hub
 - Containers = Docker + Docker-Compose
 - Orquestração de containers = Kubernetes
 - Cobertura de código = SonarCloud
 - Pipeline = Github Actions
 - Deploy = Terraform
 - Autenticação JWT

Fluxograma:
https://www.figma.com/board/iuxlSZqvof7H4K2m6uKLpt/Hackaton-FIAP---HackaMed?node-id=0-1&t=q9p1DjqBWClIjS6R-1

Video explicando a estrutura:
(adicionar video)

## Como começar

Clone o repositório na sua máquina;

Para executa-lo você tem 3 opções:
- Você pode executa-lo com o docker-compose (mais abaixo deixarei alguns passos para fazer junto da documentação do kubernetes)
- Executa-lo pelo IIS normalmente no visual studio, porém vai ser necessário ter o MongoDB instalado na máquina e fazer alguma alteração no appsettings do projeto caso seu MongoDB tenha senha.
- Executa-lo via Kubernetes (mais abaixo deixarei alguns passos para fazer junto da documentação do kubernetes)

Vou deixar disponível a collection do postman que usei para testar os fluxos dentro da Pasta do Projeto para facilitar a separação e testes do projeto.
Basta importa-la no seu postman e alterar o endpoint das requisições para a sua respectiva porta de projeto

Com o projeto rodando você pode acessar o swagger para ver as requisições disponíveis. Lembrando que foi implementado a autenticação JWT então a maioria delas vão estar bloqueadas para chamada.

Para obter o token de jwt você precisa gerar os dois primeiros usuários no banco de dados utilizando a requisição "SeedUsuarios":
![image](https://github.com/user-attachments/assets/0ce4be46-8fd5-4923-bd8b-a92938c731aa)

Com os usuarios gerados você vai poder ir até o método de Login e realizar a requisição para obter o token de acesso:
  Usuário médico: 
  - CRM: 556584
  - Senha: Teste@123
 
  Usuário paciente: 
  - CPF: 45926744807
  - Senha: Teste@123

![image](https://github.com/user-attachments/assets/da1b9a15-066a-45a7-9a67-4d9857dd493d)

Com o token você pode ir até o topo da página e acessar o botão de autorização do swagger:
![image](https://github.com/user-attachments/assets/d0cd040e-9e32-46f9-81d2-8ce42d6a4499)

Basta adicionar o token copiado nessa área:

![image](https://github.com/user-attachments/assets/ebce3ffd-f7ed-4ac4-8737-ae58a4887492)

A partir daí todas as requisições estarão disponíveis.

No caso do postman não vai ser muito diferente, você deve fazer o seed dos usuários e depois realizar o login:

![image](https://github.com/user-attachments/assets/4a07260a-32ed-48df-9718-c3a661e72285)

Com o token basta adiciona-lo como authorize nas requisições:

![image](https://github.com/user-attachments/assets/7a8a0579-6cd8-4a70-b3ce-18b00f7d2f16)


Hoje os fluxos já estão separados por Perfil, recomendo acessar como médico e criar seus horários de agendamento inicialmente, depois entrar como paciente e cadastrar seu prontuário e solicitar consultas.

![image](https://github.com/user-attachments/assets/16b76fc5-6bb0-4ddc-80fb-33de2255a27c)

A ideia foi fazer cada requisição uma parte individual então elas são reútilizáveis em alguns momentos, visto que a intenção é transformar esse monolito em microsserviços posteriomente. Caso fique perdido em relação qual a ordem do que, basta dar uma lida no fluxograma do figma que ele exemplifica o fluxo de utilização.

Feito isso você já está apto para utilizar a API tranquilamente.


## Rodando ambiente com Docker

### Pré-Requisitos
* Possuir o docker instalado:
    https://www.docker.com/products/docker-desktop/

Acesse o diretório em que o repositório foi clonado através do terminal e
execute os comandos:
 - `docker-compose build` para compilar imagens, criar containers etc.
 - `docker-compose up` para criar os containers do banco de dados e do projeto

### Iniciando e finalizando containers
Para inicializar execute o comando `docker-compose start` e
para finalizar `docker-compose stop`

Lembrando que se você for rodar pelo visual studio fica bem mais simplificado, basta estar com o docker desktop aberto na máquina e escolher a opção abaixo (lembre-se de modificar o arquivo docker-compose com suas credenciais AWS para conseguir utilizar a funcionalidade de upload de arquivos):

![image](https://github.com/user-attachments/assets/a90986a9-d1bc-4b59-a009-059f13a40582)


### Acessar swagger
Após a subida dos containers basta acessar: 

Api HackaMed
- http://localhost:7007/swagger/index.html
- https://localhost:7008/swagger/index.html


## Rodando ambiente Kubernetes
* Possuir kubernetes instalados:
    https://kubernetes.io/pt-br/docs/setup/

Com o kubernetes instalado corretamente basta ir até a pasta k8s e executar o seguinte comando:
- `kubectl apply -f` . (serão executados todos os arquivos da pasta iniciando suas configurações)
- `kubectl get deploy` (para ver os deploys que subiram, a API e o banco de dados no nosso caso);
- `kubectl get Pods` (para ver os respectivos pods)
- `kubectl get Svc` (para ver a configuração dos services dos pods, aqui você consegue pegar a porta ou endpoint para acessar seu container)

![image](https://github.com/user-attachments/assets/b336de8e-8774-4025-a8d3-256dc47317f8)

Caso esteja tendo dificuldades para acessar a respectiva porta você pode utilizar esse comando localmente para gerar um acesso em uma porta de sua escolha, basta abrir o cmd e executar:
`kubectl port-forward deployment/hackamed 7003:80 7004:443`

Para finalizar os pods e os deploys você pode executar o seguinte comando:
`kubectl delete -f` . (serão deletadas todas as configurações dos arquivos iniciados, finalizando assim os pods)


 
