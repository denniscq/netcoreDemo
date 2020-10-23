
docker build --no-cache  -f  .\Dennis.Ordering.API\Dockerfile -t dennis-ordering-api:1.0.0 .
docker build --no-cache  -f  .\Dennis.Mobile.Gateway\Dockerfile -t dennis-mobile-gateway:1.0.0 .
docker build --no-cache  -f  .\Dennis.HealthChecksHost\Dockerfile -t dennis-healthcheckshost:1.0.0 .
docker build --no-cache  -f  .\Dennis.Mobile.ApiAggregator\Dockerfile -t dennis-mobile-apiaggregator:1.0.0 .
