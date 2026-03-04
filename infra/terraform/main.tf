terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0"
    }
  }
}

provider "azurerm" {
  features {}
  subscription_id = var.subscription_id
}

# Resource Group
resource "azurerm_resource_group" "skyops" {
  name     = "rg-skyops"
  location = var.location
}

# Azure Container Registry
resource "azurerm_container_registry" "skyops" {
  name                = "skyopsacr${var.suffix}"
  resource_group_name = azurerm_resource_group.skyops.name
  location            = azurerm_resource_group.skyops.location
  sku                 = "Basic"
  admin_enabled       = true
}

# AKS Cluster
resource "azurerm_kubernetes_cluster" "skyops" {
  name                = "aks-skyops"
  location            = azurerm_resource_group.skyops.location
  resource_group_name = azurerm_resource_group.skyops.name
  dns_prefix          = "skyops"

  default_node_pool {
    name       = "default"
    node_count = 1
    vm_size    = "Standard_B2s"
  }

  identity {
    type = "SystemAssigned"
  }
}

# Allow AKS to pull from ACR
resource "azurerm_role_assignment" "aks_acr" {
  principal_id                     = azurerm_kubernetes_cluster.skyops.kubelet_identity[0].object_id
  role_definition_name             = "AcrPull"
  scope                            = azurerm_container_registry.skyops.id
  skip_service_principal_aad_check = true
}
