# DevOps Infrastructure: Vagrant + Ansible + Docker Swarm (.NET 8)

## Architecture Overview
- **Infrastructure Provisioning (Vagrant):** Two Ubuntu 22.04 nodes (`swarm-manager` on `192.168.56.11` and `swarm-worker` on `192.168.56.12`).
- **Configuration Management (Ansible):** Automated Docker Engine installation, Swarm initialization, and worker node joining (`ansible/setup-swarm.yml`).
- **Orchestration (Docker Swarm Stack):**
  - `reverse-proxy`: Nginx routing `/` to frontend and `/api/` to backend.
  - `backend`: 2 replicas of ASP.NET Core 8 Web API connected to PostgreSQL.
  - `db`: PostgreSQL 16 with persistent Docker volume.
  - `frontend`: Web UI testing load balancing across `.NET` replicas.
  - `monitoring`: Portainer CE on port `9000`.
- **CI/CD Pipeline:** GitHub Actions workflow (`.github/workflows/deploy.yml`) for automated `.NET` image builds and zero-downtime Swarm rolling updates.

## Quick Start
1. `vagrant up`
2. `ansible-playbook -i ansible/inventory.ini ansible/setup-swarm.yml`
3. `docker stack deploy -c docker-stack.yml app`
