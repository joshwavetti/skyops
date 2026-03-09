# ☁️ SkyOps — Cloud-Native Weather Dashboard

A production-grade, cloud-native weather dashboard built with C# .NET, Blazor, and a full DevOps pipeline on Azure Kubernetes Service (AKS).

![Architecture](docs/architecture.png)

## 🌟 Features

- 🌤 Real-time weather data and 5-day forecast for any city
- 🏗 Microservices architecture with BFF pattern
- 🐳 Fully containerized with Docker
- ☸️ Deployed on Azure Kubernetes Service (AKS)
- 🔄 GitOps with ArgoCD — every git push auto-deploys
- 🚀 CI/CD pipeline with GitHub Actions
- 🏗 Infrastructure as Code with Terraform

## 🏛 Architecture

```
Browser (Blazor WASM)
       ↓
GatewayApi (BFF) ←── Public LoadBalancer
       ↓
WeatherApi ←── Internal ClusterIP
       ↓
OpenWeatherMap API
```

### Services

| Service       | Technology            | Description                           |
| ------------- | --------------------- | ------------------------------------- |
| `frontend`    | Blazor WASM + Nginx   | Weather dashboard UI                  |
| `gateway-api` | C# .NET 9 Minimal API | BFF — single entry point for frontend |
| `weather-api` | C# .NET 9 Minimal API | Fetches data from OpenWeatherMap      |

## 🛠 Tech Stack

| Category               | Technology                     |
| ---------------------- | ------------------------------ |
| **Backend**            | C# .NET 9 Minimal API          |
| **Frontend**           | Blazor WebAssembly             |
| **Containerization**   | Docker, Docker Compose         |
| **Container Registry** | Azure Container Registry (ACR) |
| **Orchestration**      | Kubernetes (AKS)               |
| **Infrastructure**     | Terraform                      |
| **GitOps**             | ArgoCD                         |
| **CI/CD**              | GitHub Actions                 |
| **Cloud**              | Microsoft Azure                |

## 📁 Project Structure

```
skyops/
├── src/
│   ├── WeatherApi/          # Microservice 1 — weather data
│   ├── GatewayApi/          # Microservice 2 — BFF gateway
│   └── Frontend/            # Blazor WASM dashboard
├── infra/
│   └── terraform/           # AKS, ACR, resource group
├── k8s/
│   ├── weather-api/         # K8s manifests
│   ├── gateway-api/         # K8s manifests
│   ├── frontend/            # K8s manifests
│   └── argocd/              # ArgoCD application
├── .github/
│   └── workflows/
│       └── ci.yml           # GitHub Actions pipeline
└── docker-compose.yml       # Local development
```

## 🚀 CI/CD Pipeline

Every push to `main` triggers the following pipeline:

```
git push
    → GitHub Actions
        → Build 3 Docker images
        → Push to Azure Container Registry
        → Update image tags in k8s manifests
        → Commit updated manifests
            → ArgoCD detects change
                → Auto-deploys to AKS
```

## 🏃 Running Locally

### Prerequisites

- .NET 9 SDK
- Docker Desktop
- OpenWeatherMap API key (free at openweathermap.org)

### Steps

1. Clone the repo:

```bash
git clone https://github.com/joshwavetti/skyops.git
cd skyops
```

2. Add your API key to `src/WeatherApi/appsettings.Development.json`:

```json
{
  "OpenWeather": {
    "ApiKey": "your_api_key_here"
  }
}
```

3. Run with Docker Compose:

```bash
docker-compose up --build
```

4. Open `http://localhost:5151`

## ☁️ Deploying to Azure

### Prerequisites

- Azure CLI
- Terraform
- kubectl

### Steps

1. Login to Azure:

```bash
az login
```

2. Provision infrastructure:

```bash
cd infra/terraform
terraform init
terraform apply
```

3. Connect kubectl:

```bash
az aks get-credentials --resource-group rg-skyops --name aks-skyops
```

4. Create the API key secret:

```bash
kubectl create secret generic weather-secret --from-literal=api-key=YOUR_API_KEY
```

5. Install ArgoCD:

```bash
kubectl create namespace argocd
kubectl apply -n argocd -f https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml
kubectl apply -f k8s/argocd/skyops-app.yaml
```

6. Push to main — GitHub Actions handles the rest! 🚀

## 💰 Azure Cost Management

This project is designed to minimize Azure costs:

- Single node AKS cluster (`Standard_B2ls_v2`)
- Basic tier ACR
- Use `terraform destroy` when not in use to stop all charges
- `terraform apply` restores everything in ~10 minutes

## 📝 License

MIT
