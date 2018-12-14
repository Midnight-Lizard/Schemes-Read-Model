pod=$(kubectl get pods --selector=app=schemes-read-model-connector -o jsonpath='{.items[0].metadata.name}');
originalRestarts=$(kubectl get -o jsonpath='{.status.containerStatuses[0].restartCount}' pod $pod);
echo "originalRestarts: $originalRestarts";
kubectl exec $pod -i -- bash -c "cat /usr/tmp/connect.offsets && rm -f /usr/tmp/connect.offsets";

# kubectl exec $pod -i -- curl -X POST localhost:8083/connectors/postgresql/restart;
kubectl exec $pod -i -- curl -X PUT localhost:8083/connectors/elasticsearch-sink/pause;

restarts=$originalRestarts

while [ "$restarts" -eq "$originalRestarts" ]
do
kubectl exec $pod -i -- bash -c "rm -f /usr/tmp/connect.offsets";
echo "waiting 5s for pod restart...";
sleep 5s;
restarts=$(kubectl get -o jsonpath='{.status.containerStatuses[0].restartCount}' pod $pod);
done
