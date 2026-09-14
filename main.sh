#!/bin/bash

git status
sleep 2000
git fetch
git pull
git status
sleep 2000
docker-compose down -v
docker-compose up -d --build 
