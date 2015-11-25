FROM nice/alpine-fsharp:2f00052c29ce34a5ce8e765b287b6e5072c1b22e	

MAINTAINER James Kirk <james.kirk84@gmail.com>

ADD . /usr/share/ld-viewer/

RUN /usr/share/ld-viewer/build.sh &&\
    cd /usr/share/ld-viewer &&\
    ls -a . | grep -v "bin" | xargs -i rm -rf {}

CMD cd /usr/share/ld-viewer/ && fsharpi bin/viewer/RunServer.fsx

EXPOSE 8083
