terraform {
  backend "s3" {
    bucket = "terraform-tfstates-eks-hackamed"
    key    = "hackaMedEKS/terraform.tfstate"
    region = "us-east-1"
  }
}