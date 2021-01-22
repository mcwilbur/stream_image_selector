<template>
  <v-container fluid>
    <v-row>
      <v-container>
            <v-radio-group v-model="index">
              <v-radio
                v-for="n in 2"
                :key="n"
                :label="`Carte ${n}`"
                :value="n">
              </v-radio>
            </v-radio-group>
      </v-container>
    </v-row>
    <v-row>
      <v-col v-for="image in images" v-bind:key="image" >        
          <img v-bind:src="'cards/'+image" v-bind:class="{active1: isActive1(image), active2: isActive2(image), active_both : isActive_both(image)}" v-on:click="setActiveImage(image)" style="max-width:300px; border-radius:15px;"/>
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
      activeImage1: '',
      activeImage2: '',
      index: 1 as number,
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
        const response = await axios.post('api/Cards', {filename: image, index: this.index});
        if (this.index === 1) {
          this.activeImage1 = image;
        } else {
          this.activeImage2 = image;
        }
      } catch (e) {
        this.showError = true;
        this.errorMessage = `Error while setting active image ${e.message}.`;
      }
    },
    isActive1(image: string) {
      return (this.activeImage1 === image && this.activeImage2 !== image) ? true : false ;
    },
    isActive2(image: string) {
      return (this.activeImage2 === image && this.activeImage1 !== image) ? true : false ;
    },
      isActive_both(image: string) {
      return (this.activeImage1 === image && this.activeImage2 === image ) ? true : false ;
    },
  },
  async created() {
    await this.fetchImages();
  },
});
</script>
<style scoped>
  .active1{
    border-color:  rgba(82,168,236,.8);
    box-shadow: 0px 0px 8px 8px rgba(82,168,236,.8);
    outline: 0 none;
  }
  .active2{
    border-color:  rgba(236, 82, 82, 0.8);
    box-shadow:  0px 0px 8px 8px rgba(236, 82, 82, 0.8);
    outline: 0 none;
  }
  .active_both{
    border-color:  rgba(167, 82, 236, 0.8);
    box-shadow:  0px 0px 8px 8px rgba(167, 82, 236, 0.8);
    outline: 0 none;
  }
</style>
