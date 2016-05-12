#include <iostream>
#include <string>
using namespace std;
void reverse(const string& a);
int main()
{
    string a, b;

    getline(cin, a);
    reverse(a);
    return 0;
}
void reverse(const string& a)
{
    size_t n = a.size();
    if(n == 1)
       cout << a << endl;
    else
    {
       cout << a[n-1];
       string b = a.substr(0, n-1);
       reverse(b);
    }
}
