cd ../
export CONN_STR='Host=localhost;Port=5432;Database=Akiana-IS;Username=postgres;Password=root'
dotnet ef database update --configuration Release
read -p "Press enter to continue"