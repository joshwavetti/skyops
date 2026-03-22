# SkyOps

A cloud-native weather dashboard built as a hands-on learning
project to get practical experience with a production-grade
Azure DevOps stack.

> **Transparency note:** This project was built with significant
> AI assistance (Claude, GitHub Copilot) as a guided learning
> exercise. It is not presented as independent original work.
> The goal was to understand the end-to-end workflow — from
> local development to a live AKS deployment — by actually
> building it, not just reading about it.

---

## What I was learning

- Provisioning AKS and ACR from scratch with Terraform
- Writing Kubernetes manifests — Deployments, Services,
  ClusterIP vs LoadBalancer
- Containerising a multi-service .NET application with Docker
  and multi-stage builds
- Setting up a GitHub Actions CI pipeline that builds and
  pushes images to ACR
- ArgoCD GitOps — automatically syncing the cluster state
  to match what is in Git
- BFF (Backend for Frontend) architecture pattern

---

## Architecture

```
Browser (Blazor WASM)
        │
        ▼
  GatewayApi  ◄── Azure LoadBalancer (public)
  (BFF layer)
        │
        ▼
  WeatherApi  ◄── ClusterIP (internal only)
        │
        ▼
  OpenWeatherMap API
```

| Service       | Technology            | Role                      |
| ------------- | --------------------- | ------------------------- |
| `frontend`    | Blazor WASM + Nginx   | Weather dashboard UI      |
| `gateway-api` | C# .NET 9 Minimal API | Single public entry point |
| `weather-api` | C# .NET 9 Minimal API | Fetches weather data      |

---

## Tech stack

| Area          | Technology                     |
| ------------- | ------------------------------ |
| Backend       | C# .NET 9 Minimal API          |
| Frontend      | Blazor WebAssembly             |
| Containers    | Docker, Docker Compose         |
| Registry      | Azure Container Registry (ACR) |
| Orchestration | Kubernetes (AKS)               |
| IaC           | Terraform                      |
| GitOps        | ArgoCD                         |
| CI/CD         | GitHub Actions                 |
| Cloud         | Microsoft Azure                |

---

## CI pipeline

On every push to `main`, GitHub Actions:

1. Builds Docker images for all three services
2. Pushes tagged images to Azure Container Registry
3. Updates image tags in the Kubernetes manifests
4. Commits the updated manifests back to the repo

ArgoCD then detects the manifest change and syncs the
cluster automatically.

> Note: The CI pipeline requires Azure secrets
> (ACR credentials, service principal) configured as
> GitHub repository secrets to run successfully.
> It will fail without them — this is expected when
> running from a fork or fresh clone.

---

## Running locally

Prerequisites: .NET 9 SDK, Docker Desktop,
free OpenWeatherMap API key

```bash
git clone https://github.com/joshwavetti/skyops.git
cd skyops
```

Add your API key to
`src/WeatherApi/appsettings.Development.json`:

```json
{
  "OpenWeather": {
    "ApiKey": "your_api_key_here"
  }
}
```

```bash
docker-compose up --build
```

Open http://localhost:5151

---

## Deploying to Azure

Prerequisites: Azure CLI, Terraform, kubectl

```bash
# 1. Login
az login

# 2. Provision infrastructure
cd infra/terraform
terraform init
terraform apply

# 3. Connect kubectl to the cluster
az aks get-credentials \
  --resource-group rg-skyops \
  --name aks-skyops

# 4. Store API key as Kubernetes secret
kubectl create secret generic weather-secret \
  --from-literal=api-key=YOUR_API_KEY

# 5. Install ArgoCD
kubectl create namespace argocd
kubectl apply -n argocd \
  -f https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml
kubectl apply -f k8s/argocd/skyops-app.yaml
```

---

## Known limitations and honest notes

- README was largely AI-generated in its original form and
  contained inaccuracies (e.g. claims about parallel builds
  that were not implemented). This has been corrected.
- Architecture scaffolding was AI-assisted.
- Helm is not yet used — raw manifests only. This is a
  known gap I plan to address in a follow-up project.
- The CI pipeline will fail without Azure secrets configured —
  this is documented and expected.

---

## What's next

This project gave me a complete end-to-end map of the Azure
cloud-native stack. My next projects will be built with
progressively less AI assistance, going deeper on:

- Terraform modules and remote state management
- Helm charts for Kubernetes package management
- Proper Kubernetes networking and ingress controllers
- Observability — Prometheus and Grafana

---

## License

MIT
