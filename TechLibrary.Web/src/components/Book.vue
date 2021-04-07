<template>

    <div v-if="book" style="display:flex;align:center;">
        <b-card :title="book.title"
                :img-src="book.thumbnailUrl"
                img-alt="Image"
                img-top
                tag="article"
                style="max-width: 30rem;"
                class="mb-2">
            <b-card-text>
                {{ book.descr }}
            </b-card-text>
            
            
            <div style="display:flex;justify-content: space-between;max-width: 200px;margin:0 auto;padding: 10px 0;">
               <b-button  :pressed.sync="isEditMode" variant="primary" @click="getCurrentDetails">Edit</b-button> 
               <b-button to="/" variant="primary">Back</b-button>
            </div>
            
        </b-card>
        <div v-if="isEditMode">
            <b-card>
                    <b-form-input v-model="title" placeholder="Title"></b-form-input><br/>
                    <b-form-input v-model="imageUrl" placeholder="Thumbnail URL"></b-form-input><br/>
                    <b-form-input v-model="shortDescription" placeholder="Short Description"></b-form-input><br/>
                    <div style="display:flex;justify-content: space-between;margin:0 auto;padding: 10px 0;">
                        <b-button @click="updateBookDetails" variant="primary">UPDATE</b-button> 
                        <b-button @click="cancelUpdate" variant="primary">CANCEL</b-button>
                    </div>
                    
            </b-card>
        </div>
        
    </div>
    
</template>

<script>
    import axios from 'axios';
    

    export default {
        name: 'Book',
        props: ["id"],
        data: () => ({
            book: null,
            isEditMode: false,
            imageUrl: null,
            shortDescription: '',
            title: ''
        }),
        mounted() {
            
            const options = {
                    url: "https://localhost:5001/books/getbyid",
                    json: true,
                    method: 'PUT',
                    timeout: 5000,
                    headers: {
                    'Content-Type': 'application/json',
                    },
                    data:
                    {
                        "BookId": this.id
                    }
                };
                
                    axios(options)
                    .then((response) => {
                        this.book = response.data;
                        if(!this.book)
                        {
                            this.isNewBook = true;
                        }
                    });
        },
        methods: {
            getCurrentDetails()
            {
                this.title = this.book.title;
                this.imageUrl = this.book.thumbnailUrl;
                this.shortDescription = this.book.descr;
                
            },
            updateBookDetails() {
                const options = {
                    url: "https://localhost:5001/books/update",
                    json: true,
                    method: 'PUT',
                    timeout: 5000,
                    headers: {
                    'Content-Type': 'application/json',
                    },
                    data:
                    {
                        "BookId": this.id,
                        "ShortDescription": this.shortDescription,
                        "ThumbnailUrl": this.imageUrl
                    }
                };
                
                    axios(options)
                    .then((response) => {
                        console.log(response.data)
                        this.book = response.data;
                        this.isEditMode = false;
                    });
            },
            cancelUpdate() {
                this.isEditMode = false;
            }
        }
    }
</script>