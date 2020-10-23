kubectl create configmap dennis-config --from-env-file=env.txt -o yaml --dry-run | kubectl apply -f - 
kubectl create configmap dennis-ordering-api-config --from-file=dennis-ordering-api/configs -o yaml --dry-run | kubectl apply -f - 
kubectl create configmap dennis-mobile-apiaggregator-config --from-file=dennis-mobile-apiaggregator/configs -o yaml --dry-run | kubectl apply -f - 
kubectl create configmap dennis-mobile-gateway-config --from-file=dennis-mobile-gateway/configs -o yaml --dry-run | kubectl apply -f - 
kubectl create configmap dennis-healthcheckshost-config --from-file=dennis-healthcheckshost/configs -o yaml --dry-run | kubectl apply -f - 

helm install dennis-ordering-api .\charts\dennis-ordering-api -n default
helm install dennis-mobile-apiaggregator .\charts\dennis-mobile-apiaggregator -n default
helm install dennis-mobile-gateway .\charts\dennis-mobile-gateway -n default
helm install dennis-healthcheckshost  .\charts\dennis-healthcheckshost -n default

"Any key to exit"  ;
Read-Host | Out-Null ;
Exit