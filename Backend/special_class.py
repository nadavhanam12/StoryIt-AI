import ftplib


class FTP_TLS_UNROUTABLE_HOST(ftplib.FTP_TLS, object):
    def makepasv(self):
        _, port = super(FTP_TLS_UNROUTABLE_HOST, self).makepasv()
        return self.host, port
