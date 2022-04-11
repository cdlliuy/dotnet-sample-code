## Steps to reproduce:

# setup env: 
1.  setup a k8s cluster 
2.  edit `build-deploy.sh` for the docker repostity 
3.  edit `sample-deployment-raw.yaml` for the image url
4.  each time,  after the source code is updated,  run `build-deploy.sh` for build and re-deploy


# check graceful shutdown behaviour

run ./gracefulshutdown.sh yingtestapp-raw


