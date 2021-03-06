#!/bin/bash
tag=$1
if [ -z "$tag" ]; then
 tag="latest"
fi
rm -rf ./build/*
dotnet build "yingwebappdemo.csproj" -c Release -o ./build
docker build -t cdlliuy/yingtestappwindows:$tag -f ./Dockerfile.windows .
#docker push cdlliuy/yingtestappwindows:$tag

# comment below 2 lines if it is not the first run.
kubectl apply -f sample-svc-raw.yaml
kubectl apply -f sample-deployment-raw.yaml

kubectl set image deployment/yingtestapp-raw-deployment yingtestapp-raw=cdlliuy/yingtestappwindows:$tag

kubectl get pods -w --request-timeout='10s'