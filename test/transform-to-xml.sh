# netsh http show sslcert
# netsh http show urlacl
# netsh http delete urlacl url=https://localhost:8168/LoggerService
# netsh http add urlacl url=http://+:8169/ user=\Everyone

curl -XPOST -H 'Content-Type: application/x-www-form-urlencode' --data-binary @C:/Users/taoh02/Dropbox/Apps/X12.Web/test/fixtures/837/5010/UnicodeExample.txt http://localhost:62276/Hippa.svc/transform/x12/xml

curl -XPOST -H 'Content-Type: application/x-www-form-urlencode' --data-binary @/home/htao/X12.Web/test/fixtures/837/5010/UnicodeExample.txt http://localhost:8169/Hippa/transform/x12/xml
