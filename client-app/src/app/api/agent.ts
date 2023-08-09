import axios, { AxiosError, AxiosResponse } from "axios";
import { Activity, ActivityFormValues } from "../models/activity";
import { toast } from "react-toastify";
import { router } from "../router/Routes";
import { store } from "../stores/store";
import { User, UserFormValues } from "../models/user";
import { Profile } from "../models/profile";


const sleep = (delay: number) => {
    return new Promise((resolve) => {
        setTimeout(resolve, delay)
    })
}

//Öncelikle sleep adında bir fonksiyon tanımlanıyor. Bu fonksiyon, belirli bir süre (delay) bekleyen ve sonra 
//tamamlanan bir Promise döndüren bir yapıya sahiptir. setTimeout fonksiyonu kullanılarak belirtilen süre kadar
// beklenir ve sonra resolve fonksiyonu çağrılarak Promise tamamlanır.

axios.defaults.baseURL = 'http://localhost:5000/api';



axios.interceptors.request.use(config => {
    const token = store.commonStore.token;
    if(token && config.headers) config.headers.Authorization = `Bearer ${token}`; //Eğer token değeri mevcutsa ve config.headers varsa, istek başlığına "Authorization" başlığını ekler ve Bearer {token} şeklinde token değerini atar. Bu, oturum açma belirteci ile kimlik doğrulamasını sağlar.
    return config;
})

//axios.interceptors.request.use işlevi, Axios istek interceptor'ını tanımlamak için kullanılır. Bu interceptor, her bir Axios isteği 
//gönderildiğinde otomatik olarak tetiklenecek bir işlevi kabul eder.


axios.interceptors.response.use(async response => {
        await sleep(1000);
        return response;
}, (error: AxiosError) => {
    const {data, status, config} = error.response as AxiosResponse;
    
    switch (status) {
        case 400:
            if(config.method === 'get' && data.errors.hasOwnProperty('id')) {
                router.navigate('/not-found');
            }
            if(data.errors) {
                const modelStateErrors = [];
                for(const key in data.errors) {
                    if(data.errors[key]) {
                        modelStateErrors.push(data.errors[key]);
                    }
                }
                throw modelStateErrors.flat();
            } else {
                toast.error(data);
            }
            break;
        case 401:
            toast.error('unauthorised')
            break;
        case 403:
            toast.error('forbidden')
            break; 
        case 404:
            router.navigate('/not-found')
            break;  
        case 500:
            store.commonStore.setServerError(data);
            router.navigate('/server-error');
            break;               
      
    }
    return Promise.reject(error);
})

//Ardından, Axios'un interceptors.response özelliği kullanılarak bir yanıt (response) interceptörü tanımlanır. 
//Bu interceptör, sunucudan gelen yanıtı işlemek için kullanılır. .use metoduyla bir fonksiyon tanımlanır, 
//bu fonksiyon yanıtı alır ve işler.

//İnterseptör fonksiyonu içinde, await sleep(1000) ifadesi kullanılarak 1 saniyelik bir gecikme simüle edilir. 
//sleep fonksiyonu bir Promise döndürdüğü için await ifadesi kullanılır ve gecikme tamamlanana kadar beklenir.
//Ardından, sunucudan gelen yanıt return response ile geri döndürülür.

//Bu kod parçası, Axios isteklerine gecikme eklemek için kullanılabilir. Örneğin, sunucudan gelen yanıtların 
//hemen işlenmesi yerine belirli bir süre bekletilerek kullanıcı arayüzünde bir bekleme efekti oluşturulabilir.

const responseBody = <T> (response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T> (url: string) => axios.get<T>(url).then(responseBody),
    post: <T> (url: string, body: {}) => axios.post<T>(url,body).then(responseBody),
    put: <T> (url: string , body: {}) => axios.put<T>(url,body).then(responseBody),
    del: <T> (url: string) => axios.delete<T>(url).then(responseBody)
}

const Activities = {
    list: () => requests.get<Activity[]>('/activities'),
    details: (id: string) => requests.get<Activity>(`/activities/${id}`),
    create: (activity: ActivityFormValues) => requests.post<void>('/activities', activity),
    update: (activity: ActivityFormValues) => requests.put<void>(`/activities/${activity.id}`, activity),
    delete: (id: string) => requests.del<Activity>(`/activities/${id}`),
    attend: (id: string) => requests.post<Activity>(`/activities/${id}/attend`, {})
}

const Account = {
    current: () => requests.get<User>('/account'),
    login: (user: UserFormValues) => requests.post<User>('/account/login', user),
    register: (user: UserFormValues) => requests.post<User>('/account/register', user),

}

const Profiles = {
    get: (username: string) => requests.get<Profile>(`/profiles/${username}`) 
}

const agent = {
    Activities,
    Account,
    Profiles
}

export default agent;