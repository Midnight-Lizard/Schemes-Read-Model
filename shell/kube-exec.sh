#!/bin/bash
set -e
# echo "serching for pod"
pod=`kubectl get pods --selector=app=schemes-read-model -o jsonpath='{.items[0].metadata.name}'`;
kubectl exec $pod -i -- bash;