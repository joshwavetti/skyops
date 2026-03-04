variable "subscription_id" {
  description = "Azure Subscription ID"
  type        = string
}

variable "location" {
  description = "Azure region"
  type        = string
  default     = "westeurope"
}

variable "suffix" {
  description = "Unique suffix for ACR name"
  type        = string
  default     = "dev001"
}
