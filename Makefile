start: postgres pact-broker pact-broker-publish pact-broker-verify

postgres:
	docker compose up -d contract-postgres --remove-orphans

pact-broker:
	docker compose up -d contract-pact-broker --remove-orphans

pact-broker-publish:
	docker compose up -d contract-pact-broker-publish --remove-orphans

pact-broker-verify:
	docker compose up -d contract-pact-broker-verify --remove-orphans

stop:
	docker compose down