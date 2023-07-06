import { makeAutoObservable, runInAction } from "mobx";
import { Activity } from "../models/activity";
import agent from "../api/agent";
import { v4 as uuid } from 'uuid';
import $ from 'jquery';
import { format } from "date-fns";

export default class ActivityStore {
    // activities: Activity[] = [];
    activityRegistry = new Map<string, Activity>(); //activityRegistry bir Map nesnesi oluşturularak tanımlanmış. Map nesnesi, anahtar-değer çiftlerini saklayabilen bir koleksiyon nesnesidir.
    selectedActivity: Activity | undefined;
    editMode = false;
    loading = false;
    loadingInitial = false;


    constructor() {
        makeAutoObservable(this);
    }

    get activitiesByDate() {
        return Array.from(this.activityRegistry.values()).sort((a, b) =>
            a.date!.getTime() - b.date!.getTime());
    }
   

    get groupedActivities() {
        return Object.entries(    //Object.entries() bir JavaScript yöntemidir ve bir nesnenin [anahtar, değer] çiftlerini içeren bir diziye dönüştürür. Bu yöntem, bir nesnenin özelliklerini ve değerlerini döngüyle işlemek veya nesnenin içeriğini başka bir formatta kullanmak için kullanışlıdır.
            this.activitiesByDate.reduce((activities, activity) => {
                const date = format(activity.date!,'dd MMM yyyy');//toISOString() metodu, bir Date nesnesini ISO 8601 biçiminde bir dizeye dönüştürür. ISO 8601 biçimi, genel olarak tarih ve saatlerin uluslararası standart formatını temsil eder. Bu format, "YYYY-MM-DDTHH:mm:ss.sssZ" şeklindedir. "T" karakteri tarihi ve saati ayırmak için kullanılır.

                activities[date] = activities[date] ? [...activities[date], activity] : [activity];//activities nesnesi içindeki bu tarih anahtarı kontrol ediliyor.Eğer tarih anahtarı zaten varsa, ilgili anahtarın değeri bir diziye dönüştürülüyor ve activity bu diziye ekleniyor.Eğer tarih anahtarı yoksa, yeni bir anahtar oluşturuluyor ve bu anahtarın değeri activity'yi içeren bir dizi olarak atanıyor.
                return activities;
            }, {} as { [key: string]: Activity[] })  //{} as {[key: string]: Activity[]} ifadesi, bir boş nesneyi, anahtarları string ve değerleri Activity dizisi olan bir yapıya sahip olduğunu belirtmek için kullanmaktadır.
        )
    }

    get groupActivitiesByDate() {
        const activityMap: { [key: string]: Activity[] } = {};

        this.activitiesByDate.forEach(object => {
            const date = object.date!.toISOString().split('T')[0];//format(object.date!, 'dd MMM yyyy');

            if (activityMap[date]) {
                activityMap[date].push(object);
            } else {
                activityMap[date] = [object];
            }
        });

        // Gruplanmış etkinlikleri bir dizi olarak döndür
        return Object.entries(activityMap);

    }


    //Bu yöntem, activitiesByDate dizisindeki etkinlikleri tarihlerine göre gruplandırmak ve gruplanmış etkinlikleri bir dizi olarak 
    //döndürmek için kullanılabilir.

    loadActivities = async () => {
        this.selectedActivity = undefined;
        this.setLoadingInitial(true);

        try {
            const activities = await agent.Activities.list();

            activities.forEach(activity => {
                this.setActivity(activity);
            })
            this.setLoadingInitial(false);


        } catch (error) {
            console.log(error);

            this.setLoadingInitial(false);

        }
    }

    loadActivity = async (id: string) => {
        let activity = this.getActivity(id);

        if (activity) {
            this.selectedActivity = activity;
            return activity;
        }
        else {
            this.setLoadingInitial(true);
            try {
                activity = await agent.Activities.details(id);
                this.setActivity(activity);
                runInAction(() => { this.selectedActivity = activity });

                this.setLoadingInitial(false);
                return activity;
            } catch (error) {
                console.log(error);
                this.setLoadingInitial(false);
            }
        }
    }

    //loadActivity işlevi, belirli bir aktiviteyi yüklemek için agent.Activities.details(id) metodunu kullanır. İlk olarak activityRegistry'den 
    //aktiviteyi kontrol eder, eğer bulunursa selectedActivity'yi ayarlar ve aktiviteyi döndürür. 
    //Bulunamazsa, activityRegistry'yi yükleme işlemi başlatır ve hata durumunda konsola hata mesajını yazdırır.


    private getActivity = (id: string) => {
        return this.activityRegistry.get(id);
    }

    private setActivity = (activity: Activity) => {
        activity.date = new Date(activity.date!);
        // this.activities.push(activity);
        this.activityRegistry.set(activity.id, activity);
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }



    createActivity = async (activity: Activity) => {
        this.loading = true;
        activity.id = uuid();
        try {
            await agent.Activities.create(activity);
            runInAction(() => {
                this.activityRegistry.set(activity.id, activity);
                this.selectedActivity = activity;
                this.editMode = false;
                this.loading = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
            })
        }
    }

    updateActivity = async (activity: Activity) => {
        this.loading = true;
        try {
            await agent.Activities.update(activity);
            runInAction(() => {
                // this.activities = [...this.activities.filter(a => a.id !== activity.id), activity];
                this.activityRegistry.set(activity.id, activity);

                this.selectedActivity = activity;
                this.editMode = false;
                this.loading = false;
            })

        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
            })

        }
    }

    deleteActivity = async (id: string) => {
        this.loading = true;
        try {
            await agent.Activities.delete(id);
            runInAction(() => {
                // this.activities = [...this.activities.filter(a => a.id !== id)];
                this.activityRegistry.delete(id);

                this.editMode = false;
                this.loading = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
            })
        }
    }
}