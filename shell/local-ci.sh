#!/bin/sh
set -e
kubectl config use-context minikube
helm upgrade --install schemes-read-model ../kube/read-model
helm upgrade --install schemes-read-model-connector ../kube/es-connector
