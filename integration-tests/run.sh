#!/bin/bash

docker-compose rm -vf
docker-compose pull
docker-compose build
docker-compose up -d
sleep 60
#docker exec kbendtoendtests_tests bash /tests/run_docker.sh
result=$?
docker logs integrationtests_tests_1
docker-compose stop
docker-compose rm -vf
exit $result
