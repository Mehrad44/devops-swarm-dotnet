Vagrant.configure("2") do |config|
  config.vm.box = "ubuntu/jammy64"

  # First Server: Swarm Manager
  config.vm.define "manager" do |manager|
    manager.vm.hostname = "swarm-manager"
    manager.vm.network "private_network", ip: "192.168.56.11"
    manager.vm.provider "virtualbox" do |vb|
      vb.name = "swarm-manager"
      vb.memory = "2048"
      vb.cpus = 2
    end
  end

  # Second Server: Swarm Worker
  config.vm.define "worker" do |worker|
    worker.vm.hostname = "swarm-worker"
    worker.vm.network "private_network", ip: "192.168.56.12"
    worker.vm.provider "virtualbox" do |vb|
      vb.name = "swarm-worker"
      vb.memory = "2048"
      vb.cpus = 1
    end
  end
end