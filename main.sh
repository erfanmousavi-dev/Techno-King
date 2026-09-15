#!/bin/bash

git status
git fetch
git pull
git status
docker-compose down -v
docker-compose up -d --build 
