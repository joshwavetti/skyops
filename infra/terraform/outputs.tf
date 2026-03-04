output "acr_login_server" {
  value = azurerm_container_registry.skyops.login_server
}

output "acr_admin_username" {
  value = azurerm_container_registry.skyops.admin_username
}

output "resource_group_name" {
  value = azurerm_resource_group.skyops.name
}

output "aks_cluster_name" {
  value = azurerm_kubernetes_cluster.skyops.name
}
