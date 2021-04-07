<template>

    <div class="home">
        <h1>{{ msg }}</h1>
        
         <b-col lg="6" class="my-1">
         <div style="align:right">
        <b-button @click="showModal">Add Book</b-button>
        <b-modal hide-footer ref="addBook-modal" id="modal-1" title="BootstrapVue">
            <b-form-input v-model="title" placeholder="Title"></b-form-input><br/>
            <b-form-input v-model="isbn" placeholder="ISBN"></b-form-input><br/>
            <b-form-input v-model="publishedDate" placeholder="Published Date" type="date"></b-form-input><br/>
            <b-form-input v-model="imageUrl" placeholder="Thumbnail URL"></b-form-input><br/>
            <b-form-input v-model="shortDescription" placeholder="Short Description"></b-form-input><br/>
            <b-form-input v-model="longDescription" placeholder="Long Description"></b-form-input><br/>
            <b-button class="mt-3" variant="outline-danger" block @click="hideModal">Add</b-button>
        </b-modal>
    </div>
        <b-form-group
          label="Filter"
          label-for="filter-input"
          label-cols-sm="3"
          label-align-sm="right"
          label-size="sm"
          class="mb-0"
        >
          <b-input-group size="sm">
            <b-form-input
              id="filter-input"
              v-model="filter"
              type="search"
              placeholder="Type to Search"
            ></b-form-input>

            <b-input-group-append>
              <b-button :disabled="!filter" @click="filter = ''">Clear</b-button>
            </b-input-group-append>
          </b-input-group>
        </b-form-group>
      </b-col>
    
      <b-form-checkbox-group
        id="checkbox-group-1"
        v-model="selected"
        :options="filterBy"
        text-field="label"
        value-field="label"
        :aria-describedby="ariaDescribedby"
        name="flavour-1"
      ></b-form-checkbox-group>
        <b-pagination
      v-model="currentPage"
      :total-rows="rows"
      :per-page="perPage"
      aria-controls="books-table"
    ></b-pagination>
    
        <b-table id="books-table" ref="bookstable" striped hover :items="dataContext" :per-page="perPage" :current-page="currentPage" :fields="fields" responsive="sm"
        :filter="filter"
      :filter-included-fields="selected" >
            <template v-slot:cell(thumbnailUrl)="data">
                <b-img :src="data.value" thumbnail fluid></b-img>
            </template>
            <template v-slot:cell(title_link)="data">
                <b-link :to="{ name: 'book_view', params: { 'id' : data.item.bookId } }">{{ data.item.title }}</b-link>
            </template>
        </b-table>
    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'Home',
        props: { 
            msg: String
        },
        computed: {
            rows() {
                return this.rowCount
            }
        },
        data: () => ({
            currentPage: 1,
            perPage: 3,
            sortDirection: 'asc',
            filter: null,
            filterBy: ['Title','Description'],
            filterOn: [],
            selected: [],
            title: '',
            isbn: '',
            imageUrl: '',
            publishedDate: '',
            shortDescription: '',
            longDescription: '',
            test:'',
            fields: [
                { key: 'thumbnailUrl', label: 'Book Image' },
                { key: 'title_link', label: 'Book Title', sortable: true, sortDirection: 'desc' },
                { key: 'isbn', label: 'ISBN', sortable: true, sortDirection: 'desc' },
                { key: 'descr', label: 'Description', sortable: true, sortDirection: 'desc' }

            ],
            rowCount: 0
        }),
        methods: {
            dataContext(ctx, callback) {
                const options = {
                    url: "https://localhost:5001/books/getBooksByPageSize",
                    json: true,
                    method: 'PUT',
                    timeout: 5000,
                    headers: {
                    'Content-Type': 'application/json',
                    },
                    data:
                    {
                        "PageNumber": this.currentPage,
                        "PageSize": 10,
                        "FilterOn": this.selected,
                        "FilterBy": this.filter
                    }
                };
                
                    axios(options)
                    .then((response) => {
                        console.log(response.data)
                        this.rowCount = response.data.totalNumberOfBooks;
                        console.log(this.rowCount);
                        callback(response.data.bookResponses);
                    });
            },
            
            showModal() {
                this.$refs['addBook-modal'].show()
            },
            hideModal() {

                console.log('added successfully');
                const options = {
                    url: "https://localhost:5001/books/add",
                    json: true,
                    method: 'PUT',
                    timeout: 5000,
                    headers: {
                    'Content-Type': 'application/json',
                    },
                    data:
                    {
                        "Title": this.title,
                        "ISBN": this.isbn,
                        "PublishedDate": this.publishedDate,
                        "ThumbnailUrl": this.imageUrl,
                        "ShortDescr": this.shortDescription,
                        "longDescr": this.longDescription
                    }
                };
                
                    axios(options)
                    .then((response) => {
                        this.rowCount = response.data.length;
                        this.$refs.bookstable.refresh();
                    });
                this.$refs['addBook-modal'].hide()
            }
        }
    };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>

