# Monitoring

Grafana and Prometheus
```shell
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm repo add grafana https://grafana.github.io/helm-charts
helm repo update
kubectl create namespace monitoring

helm upgrade -i prometheus prometheus-community/prometheus -n monitoring -f prometheus.yaml

helm upgrade -i grafana grafana/grafana -n monitoring -f grafana.yaml
```

Debug:
```shell
kubectl get pods -n monitoring
kubectl get ingress -n monitoring
kubectl get svc -n monitoring
```

Visit https://k8s-grafana.local.test and https://k8s-prometheus.local.test