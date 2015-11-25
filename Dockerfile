FROM nhsevidence/alpine-fsharp:2f00052c29ce34a5ce8e765b287b6e5072c1b22e	

MAINTAINER James Kirk <james.kirk84@gmail.com>

COPY . /etc/ld-viewer/

RUN /etc/ld-viewer/build.sh &&\
    find /etc/ld-viewer/ -type d | grep -v 'bin' | xargs -i rm -rf {}

CMD cd /etc/ld-viewer/ && fsharpi bin/viewer/RunServer.fsx

EXPOSE 8083
