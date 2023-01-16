Vagrant.configure("2") do | config |
    config.vm.define "services" do |services|
        services.vm.box = "Ubuntu-Vagrant"
        services.vm.hostname = "services.lab"
        services.vm.network "private_network", ip: "100.200.100.200"
        services.vm.network "forwarded_port", guest: 80, host: 8081, auto_correct: true
	services.vm.provision "file", source: "./services", destination: "all-services"
	services.vm.provider "virtualbox" do |vb|
            vb.customize ["modifyvm", :id, "--memory", "4096"]
        end
    end  
    config.vm.define "head" do |head|
        head.vm.box = "Ubuntu-Vagrant"
        head.vm.hostname = "head.lab"
        head.vm.network "private_network", ip: "100.200.100.201"
	head.vm.network "forwarded_port", guest: 80, host: 8080, auto_correct: true
	head.vm.provision "file", source: "./playbook.yml", destination: "playbooks/playbook.yml"
        head.vm.provision "shell", path: "ansible-setup.sh"
        head.vm.provider "virtualbox" do |vb|
            vb.customize ["modifyvm", :id, "--memory", "1024"]
        end
    end  
end