FROM nice/ld-docker-build 

MAINTAINER James Kirk <james.kirk84@gmail.com>

COPY . /etc/ld-viewer/

RUN /etc/ld-viewer/build.sh

CMD cd /etc/ld-viewer/ && fsharpi bin/viewer/RunServer.fsx

EXPOSE 8083
