FROM squidfunk/mkdocs-material
RUN pip install mkdocs-awesome-pages-plugin 
RUN pip install mkdocs-minify-plugin 
RUN pip install mkdocs-glightbox 
RUN pip install mike 
RUN pip install mkdocs-git-revision-date-localized-plugin 

WORKDIR /docs
EXPOSE 8000

ENTRYPOINT ["mkdocs"]
CMD ["serve", "--dev-addr=0.0.0.0:8000"]
