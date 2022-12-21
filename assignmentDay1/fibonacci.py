def findfeb(n):
    f=0
    s=1
    t=0
    if(n>0):
        if(n==1):
            print(f)
        elif(n==2):
            print(f,s)
        else:
            print(f,s,end=" ")
            for i in range(3,n+1):
                t=f+s
                print(t,end=" ")
                f=s
                s=t
n=int(input("enter the limit"))
findfeb(n)

# using list
def fib(n):
    l=[0]
    a,b=0,1
    for i in range(n):

        a,b=b,a+b
        l.append(a)
    return l
print(fib(20))