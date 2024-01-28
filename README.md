
# Awesome Social Media - Guia de Execução
Este guia fornece instruções sobre como executar as aplicações Awesome Social Media, que consistem nos seguintes componentes:

1. #### AwesomeSocialMedia.APIGateway
2. #### AwesomeSocialMedia.Newsfeed
3. #### AwesomeSocialMedia.Posts
4. #### AwesomeSocialMedia.SocialGraph
5. #### AwesomeSocialMedia.Users
As aplicações se comunicam utilizando mensageria através do RabbitMQ e também fazem uso do Consul para descoberta de serviços.

# Pré-requisitos
Certifique-se de ter o Docker instalado em sua máquina antes de prosseguir. Você pode baixar e instalar o Docker [aqui](https://docs.docker.com/desktop/install/windows-install/).

# Passos para Execução
### 1. Executar o Consul
   Execute o seguinte comando para iniciar um contêiner Consul:
```bash
docker run -d -p 8500:8500 -p 8600:8600/udp --name=consul consul:1.15.4 agent -server -ui -node=server-1 -bootstrap-expect=1 -client=0.0.0.0
```

### 2. Executar o RabbitMQ
   Execute o seguinte comando para iniciar um contêiner RabbitMQ:

```bash
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.12-management
```
OU 

### 3. Executar as Aplicações via Docker Compose
Dentro da raiz do projeto, execute o seguinte comando para iniciar todas as aplicações e as ferramentas de monitoramento (Prometheus e Grafana) usando o arquivo **docker-compose.yml**:
```bash
docker-compose up -d
```
Este comando irá configurar e iniciar todos os serviços necessários para as aplicações, incluindo a configuração do RabbitMQ e a comunicação através do Consul.

Após a execução destes passos, as aplicações Awesome Social Media devem estar funcionando corretamente.

### 4. Acesso ao Prometheus e Grafana
* Prometheus: http://localhost:30090
* Grafana: http://localhost:30091 (Credenciais padrão: admin/admin)

No Grafana, configure um novo data source apontando para o Prometheus (http://prometheus:9090) e importe um dashboard para visualizar as métricas das aplicações Awesome Social Media.

### URLs de Acesso
* **API Gateway:** http://localhost:5143
* **Consul UI:** http://localhost:8500
* **RabbitMQ Management UI:** http://localhost:15672 (Credenciais padrão: guest/guest)
Certifique-se de verificar a documentação específica de cada aplicação para obter mais informações sobre como interagir com elas.

Para parar e remover os contêineres, você pode executar:

```bash
docker-compose down
```
Este guia fornece uma base para execução das aplicações. Consulte a documentação de cada aplicação para informações adicionais e configurações específicas.