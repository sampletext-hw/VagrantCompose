cd ../
export CONN_STR='Host=localhost;Port=5432;Database=Akiana-IS;Username=postgres;Password=root'
echo y | dotnet ef database drop --configuration Release
read -p "Press enter to continue"