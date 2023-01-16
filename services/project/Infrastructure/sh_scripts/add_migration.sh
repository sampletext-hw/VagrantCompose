cd ../
export DEBUG_CONNECT="Host=localhost;Port=5432;Database=Akiana-IS;Username=postgres;Password=root"
dotnet ef migrations add OnlinePayments -o Data/Migrations -s '../WebApi'
read -p "Press enter to continue"