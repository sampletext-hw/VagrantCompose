echo "Hello. Starting set up ansible."

sudo apt-add-repository ppa:ansible/ansible
sudo apt-get update
yes Y | sudo apt-get install ansible

echo "services ansible_host=100.200.100.200 ansible_ssh_user=vagrant ansible_ssh_pass=root" >> /etc/ansible/hosts
ssh-keyscan 100.200.100.200 >> ~/.ssh/known_hosts

ansible-playbook playbooks/playbook.yml -l services -u vagrant