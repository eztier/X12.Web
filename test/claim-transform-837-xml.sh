curl -XPOST -H 'Content-Type: application/x-www-form-urlencode' --data-binary @/home/htao/X12.Web/test/fixtures/Claims/InstitutionalClaim4010.txt http://localhost:8169/Hippa/claim/transform/837/xml

curl -XPOST -H 'Content-Type: application/x-www-form-urlencode' --data-binary @/home/htao/X12.Web/test/fixtures/Claims/InstitutionalClaim5010.txt http://localhost:8169/Hippa/claim/transform/837/xml

curl -XPOST -H 'Content-Type: application/x-www-form-urlencode' --data-binary @/home/htao/X12.Web/test/fixtures/Claims/ProfessionalClaim1.txt http://localhost:8169/Hippa/claim/transform/837/xml

