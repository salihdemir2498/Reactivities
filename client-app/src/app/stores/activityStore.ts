import { makeAutoObservable, runInAction } from "mobx";
import { Activity } from "../models/activity";
import agent from "../api/agent";
import {v4 as uuid} from 'uuid';


export default class ActivityStore {
    // activities: Activity[] = [];
    activityRegistry = new Map<string , Activity>(); //activityRegistry bir Map nesnesi oluşturularak tanımlanmış. Map nesnesi, anahtar-değer çiftlerini saklayabilen bir koleksiyon nesnesidir.
    selectedActivity: Activity | undefined;
    editMode = false;
    loading = false;
    loadingInitial = false;


    constructor() {
        makeAutoObservable(this);
    }

    get activitiesByDate() {
        return Array.from(this.activityRegistry.values()).sort((a,b) => 
            Date.parse(a.date) - Date.parse(b.date));
    }

    //Bu kod parçası, activityRegistry haritasındaki tüm aktiviteleri alarak, tarihlerine göre sıralanmış bir dizi döndürmektedir. 
    //activitiesByDate adında bir hesaplanmış özellik (computed property) olarak tanımlanmıştır.

    // Array.from(this.activityRegistry.values()) ifadesi, activityRegistry haritasının değerlerini bir diziye dönüştürmektedir. 
    //Bu dizi, tüm aktiviteleri içermektedir.

   //sort((a, b) => Date.parse(a.date) - Date.parse(b.date)) ifadesi, aktiviteleri tarihlerine göre sıralamaktadır. 
   //Date.parse(a.date) ve Date.parse(b.date) ifadeleri, her bir aktivitenin tarihini parsalamak için kullanılmaktadır. 
   //Karşılaştırma işlemi, tarihlerin sayısal değerlerine göre gerçekleştirilir ve sonuçta sıralanmış bir dizi elde edilir.

    loadActivities = async () => {
       this.setLoadingInitial(true);

        try {
            const activities = await agent.Activities.list();
          
                activities.forEach(activity => {
                    activity.date = activity.date.split('T')[0]; //Bu, tarih ve saat bilgisini ayırır ve sadece tarihi alır.
                    // this.activities.push(activity);
                    this.activityRegistry.set(activity.id, activity);
                  })
                  this.setLoadingInitial(false);
            
         
        } catch (error) {
            console.log(error);
            
            this.setLoadingInitial(false);
            
        }
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }

    selectActivity = (id: string) => {
        // this.selectedActivity = this.activities.find(a => a.id == id);
        this.selectedActivity = this.activityRegistry.get(id)

    }

    cancelSelectedActivity = () => {
        this.selectedActivity = undefined;
    }

    openForm = (id?: string) => {
        id ? this.selectActivity(id) : this.cancelSelectedActivity();
        this.editMode = true;
    }
    
    closeForm = () => {
        this.editMode = false;
    }

    createActivity = async (activity: Activity) => {
        this.loading = true;
        activity.id = uuid();
        try {
            await agent.Activities.create(activity);
            runInAction(() => {
                this.activityRegistry.set(activity.id,activity);
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