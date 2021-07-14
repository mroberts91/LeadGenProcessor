docker container kill $(docker ps -q -f "name=leadgen|dapr|placement")
docker container rm $(docker ps -a -q -f "name=leadgen|dapr|placement")
dapr uninstall
dapr init
