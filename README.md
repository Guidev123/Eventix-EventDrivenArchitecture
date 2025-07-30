<p align="center">
  <a href="https://dotnet.microsoft.com/" target="blank"><img src="https://upload.wikimedia.org/wikipedia/commons/e/ee/.NET_Core_Logo.svg" width="120" alt=".NET Logo" /></a>
</p>

<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">

 
</head>
<body>

  <h1>🎟️ Eventix</h1>
  <p><strong>Eventix</strong> is a production-ready <strong>modular monolith</strong> built with <strong>ASP.NET Core</strong> designed as a reference architecture for scalable and maintainable systems. It focuses on domain separation, modular design, and asynchronous communication using an <strong>event-driven architecture</strong>.</p>

  <h2>📦 Core Technologies</h2>
  <ul>
    <li>ASP.NET Core</li>
    <li>SQL Server</li>
    <li>RabbitMQ</li>
    <li>EventStoreDB</li>
    <li>Redis</li>
    <li>Seq (Structured Logging)</li>
    <li>Jaeger (Distributed Tracing)</li>
    <li>Keycloak (OAuth2 / OpenID Connect / SSO)</li>
    <li>YARP (API Gateway)</li>
    <li>Docker & Docker Compose</li>
  </ul>

  <h2>🧱 Architecture Overview</h2>
  <p>Eventix follows a <strong>Modular Monolith</strong> approach, with the following architectural patterns and principles:</p>
  <ul>
    <li>Clean Architecture</li>
    <li>Domain-Driven Design (DDD)</li>
    <li>Vertical Slice Architecture</li>
    <li>Event-Driven Architecture (Asynchronous communication with reliable messaging)</li>
    <li>Outbox & Inbox patterns for message reliability</li>
    <li>Fully decoupled modules communicating via integration events through RabbitMQ</li>
    <li>Database schema-per-module isolation</li>
    <li>Shared library for cross-cutting concerns (logging, tracing, security, etc.)</li>
    <li>RBAC-based access control with Keycloak</li>
  </ul>

  <h2>🧩 Modules</h2>
  <ul>
    <li><strong>Ticketing</strong></li>
    <li><strong>Events</strong></li>
    <li><strong>Attendance</strong></li>
    <li><strong>Users</strong></li>
  </ul>
  <p>Each module is completely isolated and applies Clean Architecture, with its own schema and message contracts.</p>

  <h2>✅ Testing & CI</h2>
  <ul>
    <li>Unit tests, integration tests, and architectural tests</li>
    <li>Code coverage > 70%</li>
    <li>CI pipeline with GitHub Actions</li>
  </ul>

  <h2>🔍 Observability</h2>
  <ul>
    <li><strong>Seq</strong> for structured log analysis</li>
    <li><strong>Jaeger</strong> for distributed tracing</li>
    <li><strong>OpenTelemetry</strong> integrated throughout services</li>
  </ul>

  <h2>🔐 Security</h2>
  <ul>
    <li><strong>OAuth2</strong> and <strong>OpenID Connect</strong> authentication</li>
    <li>Role-Based Access Control (RBAC) with <strong>Keycloak</strong></li>
  </ul>

  <h2>🚀 Getting Started</h2>
  <p>To run the system locally, use Docker Compose:</p>

  <pre><code>
services:
  eventix.api:
    image: ${DOCKER_REGISTRY-}eventixapi
    container_name: Eventix.Api
    build:
      context: .
      dockerfile: src/API/Eventix.Api/Dockerfile
    ports:
      - 5000:8080
      - 5001:8081
    depends_on:
      - sqlserver
      - eventix.rabbitmq
      - eventix.eventstoredb
 
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: Eventix.SqlServer
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong@Passw0rd
      - MSSQL_PID=Express
    ports:
      - 1433:1433
    volumes:
      - sqlserver_data:/var/opt/mssql 
 
  eventix.seq:
    image: datalust/seq:2025.2
    container_name: Eventix.Seq
    environment:
      - ACCEPT_EULA=Y
      - SEQ_FIRSTRUN_NOAUTHENTICATION=true
    ports:
      - 5341:5341
      - 8081:80
 
  eventix.redis:
    image: redis:latest
    container_name: Eventix.Redis
    restart: always
    ports:
      - 6379:6379
 
  eventix.identity:
    image: quay.io/keycloak/keycloak:latest
    container_name: Eventix.Identity
    command: start-dev
    environment:
      - KC_HEALTH_ENABLED=true
      - KEYCLOAK_ADMIN=admin
      - KEYCLOAK_ADMIN_PASSWORD=admin
    volumes:
      - ./.containers/identity:/opt/keycloak/data
      - ./.files:/opt/keycloak/data/import
    ports:
      - 18080:8080
 
  eventix.rabbitmq:
    image: rabbitmq:3-management
    container_name: Eventix.RabbitMQ
    environment:
      - RABBITMQ_DEFAULT_USER=guest
      - RABBITMQ_DEFAULT_PASS=guest
    ports:
      - 5672:5672      
      - 15672:15672
 
  eventix.eventstoredb:
    image: eventstore/eventstore:latest
    container_name: Eventix.EventStoreDB
    environment:
      - EVENTSTORE_INSECURE=true
      - EVENTSTORE_CLUSTER_SIZE=1
      - EVENTSTORE_RUN_PROJECTIONS=All
      - EVENTSTORE_START_STANDARD_PROJECTIONS=true
      - EVENTSTORE_HTTP_PORT=2113
      - EVENTSTORE_ENABLE_ATOM_PUB_OVER_HTTP=true
    ports:
      - "1113:1113"
      - "2113:2113"
    volumes:
      - eventstore_data:/var/lib/eventstore
      - eventstore_logs:/var/log/eventstore
 
  eventix.jaeger:
    image: jaegertracing/all-in-one:latest
    container_name: Eventix.Jaeger
    ports:
      - 4317:4317
      - 4318:4318
      - 16686:16686

  eventix.gateway:
    image: ${DOCKER_REGISTRY-}eventixgateway
    container_name: Eventix.Gateway
    build:
      context: .
      dockerfile: src/API/Eventix.Gateway/Dockerfile
    ports:
      - 3000:8080
      - 3001:8081
      
volumes:
  sqlserver_data:
  eventstore_data:
  eventstore_logs:
  </code></pre>
  
  <pre><code>docker compose up -d --build</code></pre>

  <h3>📁 Project Docker Services</h3>
  <p>Here’s an overview of the services defined in <code>docker-compose.yml</code>:</p>
  <ul>
    <li><code>eventix.api</code> - Main API</li>
    <li><code>eventix.gateway</code> - YARP API Gateway</li>
    <li><code>sqlserver</code> - Relational database</li>
    <li><code>eventix.seq</code> - Structured logging</li>
    <li><code>eventix.redis</code> - Caching layer</li>
    <li><code>eventix.identity</code> - Keycloak identity server</li>
    <li><code>eventix.rabbitmq</code> - Message broker</li>
    <li><code>eventix.eventstoredb</code> - Event store</li>
    <li><code>eventix.jaeger</code> - Tracing and observability</li>
  </ul>

  <h2>🌐 API Gateway</h2>
  <p>YARP is used as the API Gateway, forwarding requests to the appropriate internal APIs, enforcing auth policies and centralizing entrypoints.</p>

  <h2>📖 Why Eventix?</h2>
  <p><strong>Eventix</strong> demonstrates how to build a scalable, secure, and modular monolithic system that is easy to migrate to microservices when needed. It balances simplicity with enterprise-grade patterns, making it ideal for startups and production-grade backend reference implementations.</p>

  <h2>📌 License</h2>
  <p>This project is licensed under the MIT License.</p>

</body>
</html>
