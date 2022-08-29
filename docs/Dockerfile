FROM gmf520/mkdocs-material as build
COPY . /docs
RUN mkdocs build

FROM nginx as final
WORKDIR /usr/share/nginx/html
COPY --from=build /docs/site .
EXPOSE 80
