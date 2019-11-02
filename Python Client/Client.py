import socket, os.path, datetime, sys

# def Main():
host = '192.168.1.9'
port = 3000

s = socket.socket()
s.connect((host, port))

Sentence = "What time are you open?"
s.send(Sentence.encode('utf-8'))
s.shutdown(socket.SHUT_WR)
data = s.recv(1024).decode('utf-8')
print(data)
s.close()

# if __name__ == '__main__':
#     Main()