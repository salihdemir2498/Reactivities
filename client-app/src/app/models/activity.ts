import { Profile } from "./profile";

export interface Activity {
    id: string;
    title: string;
    date: Date | null;
    description: string;
    category: string;
    city: string;
    venue: string;
    hostUsername: string;
    isCancelled: boolean;
    isGoing: boolean;
    isHost: boolean;
    host?: Profile;
    attendees: Profile[];
  }

  export class ActivityFormValues {
    id?: string = undefined;
    title: string = '';
    category: string = '';
    description: string = '';
    date: Date | null = null;
    city: string = '';
    venue: string = '';

    constructor(activity?: ActivityFormValues) {
      if (activity) {
        this.id = activity.id;
        this.title = activity.title;
        this.category = activity.category;
        this.description = activity.description;
        this.date = activity.date;
        this.city = activity.city;
        this.venue = activity.venue;
        
      }
    }
  }

  export class Activity implements Activity {
    constructor(init?: ActivityFormValues) {
      Object.assign(this, init); //Bu satırda, yapıcı metot, "init" adında bir "ActivityFormValues" nesnesini alır ve 
      //bu nesnenin özelliklerini, "Activity" sınıfının özelliklerine kopyalar. "Object.assign" yöntemi, bir veya daha 
      //fazla kaynaktan hedefe özelliklerin kopyalanmasını sağlayan bir JavaScript yöntemidir. 
      //Bu satırda, "init" nesnesinin özellikleri, "this" yani "Activity" sınıfının özelliklerine kopyalanır. 
      //Böylece, "init" nesnesi üzerindeki değerler, "Activity" sınıfının nesnesine atanmış olur.
    }
  }
