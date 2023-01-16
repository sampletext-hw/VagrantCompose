# задача контейнера запустить Nginx с сертификатами, чтобы он пробрасывал внутрь запросы к конкретным серверам фронта и бэка
FROM nginx:latest
COPY nginx-test.conf /etc/nginx/nginx.conf