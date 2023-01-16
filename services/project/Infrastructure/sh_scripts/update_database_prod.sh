# !!! WARNING !!!
# NEVER DO THIS UNLESS YOU REALLY UNDERSTAND WHAT YOU ARE DOING

# https://stackoverflow.com/a/31938017

echo "PLEASE VERIFY UPDATING PRODUCTION DATABASE"
DEFAULT="y"
read -e -p "Proceed [Y/n/q]: " PROCEED
# adopt the default, if 'enter' given
PROCEED="${PROCEED:-${DEFAULT}}"
# change to lower case to simplify following if
PROCEED="${PROCEED,,}"
# condition for specific letter
if [ "${PROCEED}" == "q" ] ; then
  echo "Quitting"
  exit
# condition for non specific letter (ie anything other than q/y)
# if you want to have the active 'y' code in the last section
elif [ "${PROCEED}" != "y" ] ; then
  echo "Not Proceeding"
else
  echo "Proceeding"
  # do proceeding code in here
  cd ../
  export DEBUG_CONNECT="Host=localhost;Port=55432;Database=Akiana-IS;Username=postgres;Password=l2j40SzyuFq0"
  dotnet ef database update --configuration Release -s '../WebApi'
  read -p "Press enter to continue"
fi