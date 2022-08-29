FROM squidfunk/mkdocs-material

RUN pip install mkdocs-minify-plugin \
    mkdocs-glightbox \
    mkdocs-awesome-pages-plugin

WORKDIR /docs
EXPOSE 80

ENTRYPOINT ["mkdocs"]
CMD ["serve", "--dev-addr=0.0.0.0:80"]
