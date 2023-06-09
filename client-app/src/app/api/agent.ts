import axios, { AxiosError, AxiosResponse } from "axios";
import { Activity } from "../models/activity";
import { toast } from "react-toastify";
import { router } from "../router/Routes";
import { store } from "../stores/store";


const sleep = (delay: number) => {
    return new Promise((resolve) => {
        setTimeout(resolve, delay)
    })
}

//Öncelikle sleep adında bir fonksiyon tanımlanıyor. Bu fonksiyon, belirli bir süre (delay) bekleyen ve sonra 
//tamamlanan bir Promise döndüren bir yapıya sahiptir. setTimeout fonksiyonu kullanılarak belirtilen süre kadar
// beklenir ve sonra resolve fonksiyonu çağrılarak Promise tamamlanır.

axios.defaults.baseURL = 'http://localhost:5000/api';

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
    create: (activity: Activity) => requests.post<Activity>('/activities', activity),
    update: (activity: Activity) => requests.put<Activity>(`/activities/${activity.id}`, activity),
    delete: (id: string) => requests.del<Activity>(`/activities/${id}`)
}

const agent = {
    Activities
}

export default agent;