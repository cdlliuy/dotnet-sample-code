## Steps to reproduce:

# setup env: 
1.  setup a k8s cluster 
2.  edit `build-deploy.sh` for the docker repostity 
3.  edit `sample-deployment-raw.yaml` for the image url
4.  each time,  after the source code is updated,  run `build-deploy.sh` for build and re-deploy


# reproduce the error

1. get your pod ip address by "kubectl get pods -o wide"
2. exec into another pod with support "curl", and cmd "curl http://<pod-ip>/".  After 30s, "ok" will be responded. 
3. keep monitoring the logs of the target pod
```
label=yingtestapp-raw
podname=$(kubectl get pod -l app=$label -ojsonpath="{.items[0].metadata.name}")  && kubectl logs $podname -f | grep -v "pollingTasks" | tee debug.log
```
4. delete the target pod in another terminal window
```
podname=$(kubectl get pod -l app=$label -ojsonpath="{.items[0].metadata.name}")  && kubectl delete pod $podname && date
```

