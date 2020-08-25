<template>
  <v-container fluid>
    <v-row>
      <v-col v-for="image in images" v-bind:key="image" >        
          <img v-bind:src="'cards/'+image" v-bind:class="{active: activeImage == image}" v-on:click="setActiveImage(image)" style="max-width:300px; border-radius:15px;"/>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
// an example of a Vue Typescript component using Vue.extend
import Vue from 'vue';
import axios from 'axios';

export default Vue.extend({
  data() {
    return {
      loading: true,
      showError: false,
      errorMessage: 'Error while loading images.',
      images: [],
      activeImage: '',
    };
  },
  methods: {
    async fetchImages() {
      try {
        const response = await axios.get('api/Cards');
        this.images = response.data;
      } catch (e) {
        this.showError = true;
        this.errorMessage = `Error while loading images: ${e.message}.`;
      }
      this.loading = false;
    },
    async setActiveImage(image: string) {
      try {
        const response = await axios.post('api/Cards', {filename: image});
        this.activeImage = image;
      } catch (e) {
        this.showError = true;
        this.errorMessage = `Error while setting active image ${e.message}.`;
      }
    },
    isActive(image: string) {
      return (this.activeImage === image) ? true : false ;
    },
  },
  async created() {
    await this.fetchImages();
  },
});
</script>
<style scoped>
  .active{
    border-color:  rgba(82,168,236,.8);
    box-shadow: 0 0px 0px rgba(82,168,236,.8) inset, 0 0 8px rgba(82,168,236,.8);
    outline: 0 none;
  }
</style>
