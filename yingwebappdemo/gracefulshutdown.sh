#!/bin/bash
label=$1
podname=$(kubectl get pod -l app=$label -ojsonpath="{.items[0].metadata.name}")  
#podname=feedupload-7d58b9ff58-zs6zk
nohup kubectl logs $podname -f -c $label 2>&1 > $label.log  & 
sleep 5
curl localhost:5000 & 
sleep 5
kubectl delete pod $podname &
tail -f $label.log
