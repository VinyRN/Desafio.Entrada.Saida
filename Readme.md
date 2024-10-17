# Desafio Entrada Saída

## Descrição
O **Desafio Entrada Saída** é uma API desenvolvida em ASP.NET Core que processa pedidos, determina a melhor forma de embalar produtos em caixas pré-fabricadas e registra logs e eventos usando Elasticsearch e RabbitMQ. O projeto é modular, dividido em várias bibliotecas para facilitar a manutenção e escalabilidade.

## Estrutura do Projeto
O projeto está organizado em várias bibliotecas e serviços:
- **Desafio.Entrada.Saida.Api**: API principal que expõe endpoints para processamento de pedidos.
- **Desafio.Entrada.Saida.Dominio**: Define DTOs (Data Transfer Objects) e interfaces essenciais.
- **Desafio.Entrada.Saida.Servico**: Implementações de serviços para processamento de pedidos e embalagem de produtos.
- **Desafio.Entrada.Saida.Infraestrutura.Repositorios**: Implementações de repositórios para acesso a dados.
- **Desafio.Entrada.Saida.Queue**: Integração com o RabbitMQ para publicação e consumo de mensagens.

## Tecnologias Utilizadas
- **ASP.NET Core** 7.0
- **RabbitMQ** para gerenciamento de mensagens
- **Elasticsearch** e **Kibana** para monitoramento e visualização de logs
- **Serilog** para logging estruturado

## Pré-requisitos
Para executar o projeto, você precisará ter instalado:
- Docker
- .NET SDK 7.0 ou superior (para desenvolvimento e build local)

## Configuração e Execução
```bash
git clone https://github.com/seu-usuario/desafio-entrada-saida.git
cd desafio-entrada-saida

### Subir os Serviços Necessários com Docker
Elasticsearch
Execute o comando abaixo para subir o Elasticsearch:
docker run -d --name elasticsearch -p 9200:9200 -e "discovery.type=single-node" -e "ES_JAVA_OPTS=-Xms512m -Xmx512m" docker.elastic.co/elasticsearch/elasticsearch:7.9.2

### Kibana
Suba o Kibana para visualizar os logs:
docker run -d --name kibana -p 5601:5601 --link elasticsearch:elasticsearch docker.elastic.co/kibana/kibana:7.9.2


### RabbitMQ
Inicie o RabbitMQ com interface de gerenciamento:
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management

### Configuração da Aplicação
No appsettings.json da sua aplicação, configure as URLs para que a aplicação saiba onde encontrar os serviços:

{
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.Elasticsearch" ],
    "MinimumLevel": "Information",
    "Enrich": [ "FromLogContext", "WithMachineName", "WithEnvironmentUserName" ],
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "Elasticsearch",
        "Args": {
          "nodeUris": "http://localhost:9200",
          "indexFormat": "logs-desafio-entrada-saida-{0:yyyy.MM}",
          "autoRegisterTemplate": true
        }
      }
    ]
  },
  "RabbitMq": {
    "Host": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest"
  }
}

### Executar a Aplicação Localmente
Você pode executar a aplicação localmente usando o .NET CLI:

dotnet build
dotnet run --project Desafio.Entrada.Saida.Api

### Executar a Aplicação em um Container Docker
Para rodar a aplicação em um container Docker, utilize os comandos abaixo:

Construir a Imagem Docker

docker build -t desafio-entrada-saida-api .


### Executar o Container

docker run -d -p 5000:80 -p 5001:443 --name desafio-entrada-saida-api desafio-entrada-saida-api


### Acessar os Serviços
API: http://localhost:5000
Kibana: http://localhost:5601 (para visualizar logs)
RabbitMQ Management: http://localhost:15672 (usuário: guest, senha: guest)

### Estrutura do Código
O código é modularizado para facilitar o entendimento e manutenção:

Controllers: Localizados na Desafio.Entrada.Saida.Api, expõem endpoints para funcionalidades principais.

Services: Implementações de serviços de negócio na Desafio.Entrada.Saida.Servico.

Repositories: Repositórios para gerenciamento de dados em Desafio.Entrada.Saida.Infraestrutura.Repositorios.

Queue Integration: Implementações para integração com RabbitMQ em Desafio.Entrada.Saida.Queue.

### Testes e Desenvolvimento Local
Para desenvolvimento local, é possível compilar e rodar a aplicação com os comandos:

dotnet build
dotnet run --project Desafio.Entrada.Saida.Api

### Exemplo de Endpoint
POST /api/embalagem/processar-pedidos

### URL Completa:
http://localhost:5000/api/embalagem/processar-pedidos

### Cabeçalhos (Headers):
Content-Type: application/json

### Corpo da Requisição (Request Body):
Envie um JSON contendo uma lista de pedidos. Cada pedido deve conter um identificador e uma lista de produtos com suas dimensões (altura, largura e comprimento).

[
    {
        "IdPedido": 1,
        "Produtos": [
            {
                "IdProduto": 101,
                "Altura": 20,
                "Largura": 30,
                "Comprimento": 40
            },
            {
                "IdProduto": 102,
                "Altura": 40,
                "Largura": 50,
                "Comprimento": 60
            }
        ]
    },
    {
        "IdPedido": 2,
        "Produtos": [
            {
                "IdProduto": 201,
                "Altura": 10,
                "Largura": 15,
                "Comprimento": 20
            },
            {
                "IdProduto": 202,
                "Altura": 35,
                "Largura": 25,
                "Comprimento": 45
            }
        ]
    }
]
### Explicações dos Campos:
IdPedido: Identificador único do pedido.

Produtos: Lista de produtos pertencentes ao pedido.

IdProduto: Identificador único do produto.

Altura, Largura, Comprimento: Dimensões do produto em centímetros.

###Exemplo de Resposta (Response Body):
Caso a chamada seja bem-sucedida, a API retornará um JSON contendo o resultado do processamento, com informações sobre quais caixas foram utilizadas para cada pedido.
{
    "PedidosProcessados": [
        {
            "IdPedido": 1,
            "CaixasUtilizadas": [
                {
                    "IdCaixa": 1,
                    "Altura": 30,
                    "Largura": 40,
                    "Comprimento": 80,
                    "IdsProdutos": [101, 102]
                }
            ]
        },
        {
            "IdPedido": 2,
            "CaixasUtilizadas": [
                {
                    "IdCaixa": 2,
                    "Altura": 80,
                    "Largura": 50,
                    "Comprimento": 40,
                    "IdsProdutos": [201, 202]
                }
            ]
        }
    ],
    "Sucesso": true
}

###Explicações da Resposta:

PedidosProcessados: Lista de pedidos processados, cada um contendo as caixas utilizadas e os produtos alocados.

IdCaixa: Identificador da caixa utilizada.

Altura, Largura, Comprimento: Dimensões da caixa utilizada.

IdsProdutos: Lista de IDs dos produtos que foram alocados na caixa.

Sucesso: Indicador de sucesso do processamento (true ou false).

###Possíveis Erros e Respostas
Caso a chamada apresente algum problema, a API pode retornar um erro como:

400 Bad Request (Exemplo: Lista de pedidos vazia ou inválida)

{
    "Mensagem": "A lista de pedidos não pode estar vazia."
}

500 Internal Server Error (Exemplo: Erro interno durante o processamento)

{
    "Mensagem": "Erro interno ao processar os pedidos."
}


###Testando a API
Você pode usar Postman, Insomnia ou curl para testar:

Com curl:
curl -X POST http://localhost:5000/api/embalagem/processar-pedidos \
-H "Content-Type: application/json" \
-d '[
    {
        "IdPedido": 1,
        "Produtos": [
            { "IdProduto": 101, "Altura": 20, "Largura": 30, "Comprimento": 40 },
            { "IdProduto": 102, "Altura": 40, "Largura": 50, "Comprimento": 60 }
        ]
    },
    {
        "IdPedido": 2,
        "Produtos": [
            { "IdProduto": 201, "Altura": 10, "Largura": 15, "Comprimento": 20 },
            { "IdProduto": 202, "Altura": 35, "Largura": 25, "Comprimento": 45 }
        ]
    }
]'

### Melhorias Futuras
Adicionar testes unitários e de integração para garantir a robustez do sistema.

Expandir funcionalidades de gerenciamento de pedidos e logs para incluir mais detalhes.

Suporte para configuração dinâmica das filas e integração com outros serviços de mensageria.

### Contribuições
Contribuições são bem-vindas! Sinta-se à vontade para abrir issues e pull requests para melhorar o projeto.

### Licença
Este projeto é distribuído sob a licença MIT. Consulte o arquivo LICENSE para mais informações.


Desenvolvido por Vinicius Nunes

### Explicações e Considerações
- **Execução de Serviços Individualmente**: O README orienta o usuário a subir cada serviço separadamente usando comandos Docker, substituindo o uso de Docker Compose.
- **Configuração Manual**: Fornece detalhes claros sobre como configurar a aplicação para se conectar aos serviços (Elasticsearch, Kibana e RabbitMQ).
- **Execução da Aplicação**: Instruções para rodar a aplicação tanto localmente quanto em um container Docker.

Este README cobre todos os passos necessários para entender, configurar e executar o projeto sem o Docker Compose, garantindo que qualquer desenvolvedor possa seguir e configurar o ambiente de forma eficiente. Se precisar de mais detalhes ou ajustes, é só me avisar!


